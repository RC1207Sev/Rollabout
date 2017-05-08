using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace MyUtils
{
	public static class SearchTools
	{

		public static Transform FindInChildsTree(String name, Transform root)
		{
			Transform temp;

			if (root == null)
				return null;

			if (root.name == name)
				return root;

			foreach(Transform child in root)
			{
				temp = FindInChildsTree(name, child);
				if (temp != null)
					return temp;
			}

			return null;
		}

	}
}

