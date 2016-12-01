using System;
using UnityEngine;

[AddComponentMenu("")]
public class AmplifyColorBase : MonoBehaviour
{
    public float BlendAmount;
    private RenderTexture blendCacheLut;
    private bool blending;
    private float blendingTime;
    private float blendingTimeCountdown;
    private ColorSpace colorSpace = ColorSpace.Uninitialized;
    internal bool JustCopy;
    public Texture2D LutTexture;
    private Texture lutTexture3d = new Texture();
    public Texture2D LutBlendTexture;
    private Texture lutBlendTexture3d = new Texture();
    public Texture MaskTexture; 
    private Material materialBase;
    private Material materialBlend;
    private Material materialBlendCache;
    private Material materialBlendMask;
    private Material materialMask;
    private Texture2D normalLut;
    private Action onFinishBlend;
    private Shader shaderBase;
    private Shader shaderBlend;
    private Shader shaderBlendCache;
    private Shader shaderBlendMask;
    private Shader shaderMask;
    private bool use3d = new bool();

    public void BlendTo(Texture2D blendTargetLUT, float blendTimeInSec, Action onFinishBlend)
    {
        this.LutBlendTexture = blendTargetLUT;
        this.BlendAmount = 0f;
        this.onFinishBlend = onFinishBlend;
        this.blendingTime = blendTimeInSec;
        this.blendingTimeCountdown = blendTimeInSec;
        this.blending = true;
    }

    private bool CheckMaterialAndShader(Material material, string name)
    {
        if ((material == null) || (material.shader == null))
        {
            Debug.LogError("[AmplifyColor] Error creating " + name + " material. Effect disabled.");
            base.enabled = false;
        }
        else if (!material.shader.isSupported)
        {
            Debug.LogError("[AmplifyColor] " + name + " shader not supported on this platform. Effect disabled.");
            base.enabled = false;
        }
        else
        {
            material.hideFlags = HideFlags.HideAndDontSave;
        }
        return base.enabled;
    }

    private bool CheckShader(Shader s)
    {
        if (s == null)
        {
            this.ReportMissingShaders();
            return false;
        }
        if (!s.isSupported)
        {
            this.ReportNotSupported();
            return false;
        }
        return true;
    }

    private bool CheckShaders()
    {
        return (((this.CheckShader(this.shaderBase) && this.CheckShader(this.shaderBlend)) && (this.CheckShader(this.shaderBlendCache) && this.CheckShader(this.shaderMask))) && this.CheckShader(this.shaderBlendMask));
    }

    private bool CheckSupport()
    {
        if (SystemInfo.supportsImageEffects && SystemInfo.supportsRenderTextures)
        {
            return true;
        }
        this.ReportNotSupported();
        return false;
    }

    private void CreateHelperTextures()
    {
        int width = 0x400;
        int height = 0x20;
        this.ReleaseTextures();
        RenderTexture texture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear) {
            hideFlags = HideFlags.HideAndDontSave
        };
        this.blendCacheLut = texture;
        this.blendCacheLut.name = "BlendCacheLut";
        this.blendCacheLut.wrapMode = TextureWrapMode.Clamp;
        this.blendCacheLut.useMipMap = false;
        this.blendCacheLut.anisoLevel = 0;
        this.blendCacheLut.Create();
        Texture2D textured = new Texture2D(width, height, TextureFormat.RGB24, false, true) {
            hideFlags = HideFlags.HideAndDontSave
        };
        this.normalLut = textured;
        this.normalLut.name = "NormalLut";
        this.normalLut.hideFlags = HideFlags.DontSave;
        Color32[] colors = new Color32[width * height];
        for (int i = 0; i < 0x20; i++)
        {
            int num4 = i * 0x20;
            for (int j = 0; j < 0x20; j++)
            {
                int num6 = num4 + (j * width);
                for (int k = 0; k < 0x20; k++)
                {
                    float num8 = ((float) k) / 31f;
                    float num9 = ((float) j) / 31f;
                    float num10 = ((float) i) / 31f;
                    byte r = (byte) (num8 * 255f);
                    byte g = (byte) (num9 * 255f);
                    byte b = (byte) (num10 * 255f);
                    colors[num6 + k] = new Color32(r, g, b, 0xff);
                }
            }
        }
        this.normalLut.SetPixels32(colors);
        this.normalLut.Apply();
    }

    private void CreateMaterials()
    {
        this.SetupShader();
        this.ReleaseMaterials();
        this.materialBase = new Material(this.shaderBase);
        this.materialBlend = new Material(this.shaderBlend);
        this.materialBlendCache = new Material(this.shaderBlendCache);
        this.materialMask = new Material(this.shaderMask);
        this.materialBlendMask = new Material(this.shaderBlendMask);
        this.CheckMaterialAndShader(this.materialBase, "BaseMaterial");
        this.CheckMaterialAndShader(this.materialBlend, "BlendMaterial");
        this.CheckMaterialAndShader(this.materialBlendCache, "BlendCacheMaterial");
        this.CheckMaterialAndShader(this.materialMask, "MaskMaterial");
        this.CheckMaterialAndShader(this.materialBlendMask, "BlendMaskMaterial");
        if (base.enabled)
        {
            this.CreateHelperTextures();
        }
    }

    private void OnDisable()
    {
        this.ReleaseMaterials();
        this.ReleaseTextures();
    }

    private void OnEnable()
    {
        if (this.CheckSupport())
        {
            this.CreateMaterials();
            if (((this.LutTexture != null) && (this.LutTexture.mipmapCount > 1)) || ((this.LutBlendTexture != null) && (this.LutBlendTexture.mipmapCount > 1)))
            {
                Debug.LogError("[AmplifyColor] Please disable \"Generate Mip Maps\" import settings on all LUT textures to avoid visual glitches. Change Texture Type to \"Advanced\" to access Mip settings.");
            }
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        this.BlendAmount = Mathf.Clamp01(this.BlendAmount);
        if (this.colorSpace != QualitySettings.activeColorSpace)
        {
            this.CreateMaterials();
        }
        bool flag = ValidateLutDimensions(this.LutTexture);
        bool flag2 = ValidateLutDimensions(this.LutBlendTexture);
        if ((this.JustCopy || !flag) || !flag2)
        {
            Graphics.Blit(source, destination);
        }
        else if (((this.LutTexture == null) && (this.lutTexture3d == null)) || ((this.BlendAmount == 1f) && (this.LutBlendTexture == null)))
        {
            Graphics.Blit(source, destination);
        }
        else
        {
            Material materialBlendMask;
            int pass = !base.GetComponent<Camera>().hdr ? 0 : 1;
            bool flag3 = !(this.BlendAmount == 0f);
            bool flag4 = flag3 || (flag3 && ((this.LutBlendTexture != null) || (this.lutBlendTexture3d != null)));
            bool flag5 = flag4 && !this.use3d;
            if (flag4)
            {
                if (this.MaskTexture != null)
                {
                    materialBlendMask = this.materialBlendMask;
                }
                else
                {
                    materialBlendMask = this.materialBlend;
                }
            }
            else if (this.MaskTexture != null)
            {
                materialBlendMask = this.materialMask;
            }
            else
            {
                materialBlendMask = this.materialBase;
            }
            materialBlendMask.SetFloat("_lerpAmount", this.BlendAmount);
            if (this.MaskTexture != null)
            {
                materialBlendMask.SetTexture("_MaskTex", this.MaskTexture);
            }
            if (flag5)
            {
                this.materialBlendCache.SetFloat("_lerpAmount", this.BlendAmount);
                this.materialBlendCache.SetTexture("_RgbTex", this.LutTexture);
                this.materialBlendCache.SetTexture("_LerpRgbTex", (this.LutBlendTexture != null) ? this.LutBlendTexture : this.normalLut);
                Graphics.Blit(this.LutTexture, this.blendCacheLut, this.materialBlendCache);
                materialBlendMask.SetTexture("_RgbBlendCacheTex", this.blendCacheLut);
            }
            else if (!this.use3d)
            {
                if (this.LutTexture != null)
                {
                    materialBlendMask.SetTexture("_RgbTex", this.LutTexture);
                }
                if (this.LutBlendTexture != null)
                {
                    materialBlendMask.SetTexture("_LerpRgbTex", this.LutBlendTexture);
                }
            }
            Graphics.Blit(source, destination, materialBlendMask, pass);
            if (flag5)
            {
                this.blendCacheLut.DiscardContents();
            }
        }
    }

    private void ReleaseMaterials()
    {
        if (this.materialBase != null)
        {
            UnityEngine.Object.DestroyImmediate(this.materialBase);
            this.materialBase = null;
        }
        if (this.materialBlend != null)
        {
            UnityEngine.Object.DestroyImmediate(this.materialBlend);
            this.materialBlend = null;
        }
        if (this.materialBlendCache != null)
        {
            UnityEngine.Object.DestroyImmediate(this.materialBlendCache);
            this.materialBlendCache = null;
        }
        if (this.materialMask != null)
        {
            UnityEngine.Object.DestroyImmediate(this.materialMask);
            this.materialMask = null;
        }
        if (this.materialBlendMask != null)
        {
            UnityEngine.Object.DestroyImmediate(this.materialBlendMask);
            this.materialBlendMask = null;
        }
    }

    private void ReleaseTextures()
    {
        if (this.blendCacheLut != null)
        {
            UnityEngine.Object.DestroyImmediate(this.blendCacheLut);
            this.blendCacheLut = null;
        }
        if (this.normalLut != null)
        {
            UnityEngine.Object.DestroyImmediate(this.normalLut);
            this.normalLut = null;
        }
    }

    private void ReportMissingShaders()
    {
        Debug.LogError("[AmplifyColor] Error initializing shaders. Please reinstall Amplify Color.");
        base.enabled = false;
    }

    private void ReportNotSupported()
    {
        Debug.LogError("[AmplifyColor] This image effect is not supported on this platform. Please make sure your Unity license supports Full-Screen Post-Processing Effects which is usually reserved forn Pro licenses.");
        base.enabled = false;
    }

    private void SetupShader()
    {
        this.colorSpace = QualitySettings.activeColorSpace;
        string str = (this.colorSpace == ColorSpace.Linear) ? "Linear" : "";
        string str2 = "";
        this.shaderBase = Shader.Find("Hidden/Amplify Color/Base" + str + str2);
        this.shaderBlend = Shader.Find("Hidden/Amplify Color/Blend" + str + str2);
        this.shaderBlendCache = Shader.Find("Hidden/Amplify Color/BlendCache");
        this.shaderMask = Shader.Find("Hidden/Amplify Color/Mask" + str + str2);
        this.shaderBlendMask = Shader.Find("Hidden/Amplify Color/BlendMask" + str + str2);
    }

    private void Update()
    {
        if (this.blending)
        {
            this.BlendAmount = (this.blendingTime - this.blendingTimeCountdown) / this.blendingTime;
            this.blendingTimeCountdown -= Time.smoothDeltaTime;
            if (this.BlendAmount >= 1f)
            {
                this.LutTexture = this.LutBlendTexture;
                this.BlendAmount = 0f;
                this.blending = false;
                this.LutBlendTexture = null;
                if (this.onFinishBlend != null)
                {
                    this.onFinishBlend();
                }
            }
        }
        else
        {
            this.BlendAmount = Mathf.Clamp01(this.BlendAmount);
        }
    }

    public static bool ValidateLutDimensions(Texture2D lut)
    {
        if (lut != null)
        {
            if ((lut.width / lut.height) != lut.height)
            {
                Debug.LogWarning("[AmplifyColor] Lut " + lut.name + " has invalid dimensions.");
                return false;
            }
            if (lut.anisoLevel != 0)
            {
                lut.anisoLevel = 0;
            }
        }
        return true;
    }

    public bool IsBlending
    {
        get
        {
            return this.blending;
        }
    }

    public bool WillItBlend
    {
        get
        {
            return (((this.LutTexture != null) && (this.LutBlendTexture != null)) && !this.blending);
        }
    }
}

