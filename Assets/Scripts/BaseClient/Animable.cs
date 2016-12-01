using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RS2Sharp
{
	public class Animable : NodeSub
	{
		public virtual void renderAtPoint(int i, int j, int k, int l, int i1, int xOffset, int j1, int yOffset, int k1, int zOffset,
		                              int l1, int i2, RuneObject runeObj)
		{
			Debug.Log ("renderAtPoint");
			Model model = getRotatedModel();
			if (model != null) {
				modelHeight = model.modelHeight;
				if(this is Player) model.renderAtPoint (i, j, k, l, i1, xOffset, j1, yOffset, k1, zOffset, l1, i2, (this as Player).playerObj);
				else if(this is NPC) model.renderAtPoint (i, j, k, l, i1, xOffset, j1, yOffset, k1, zOffset, l1, i2, (this as NPC).npcObj);
				else model.renderAtPoint (i, j, k, l, i1, xOffset, j1, yOffset, k1, zOffset, l1, i2, runeObj);
			}
		}
		
		public void render(int x, int y, int orientation, int j, int k, int l, int i1, int height, int key, RuneObject runeObj) {
			Model model = null;
			if(this is Animable_Sub5) model = (this as Animable_Sub5).getRotatedModel();
			else if(this is Model) model = this as Model;
			if (model != null)
			{
				modelHeight = model.modelHeight;
				if(this is Player) model.renderAtPoint (orientation, j, k, l, i1, x, height, y, key, (this as Player).playerObj);
				else if(this is NPC) model.renderAtPoint (orientation, j, k, l, i1, x, height, y, key, (this as NPC).npcObj);
				else model.renderAtPoint (orientation, j, k, l, i1, x, height, y, key, runeObj);
			}
			else
			{
			}
		}
		
		public virtual Model getRotatedModel()
		{
			Debug.Log ("Shouldn't be here");
			return null;
		}

		public Animable()
		{
			modelHeight = 1000;
		}

		public VertexNormal[] vertexNormalOffset;
		public int modelHeight;
	}
}
