using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Utils
{ 
	public static t FindComp<t>(Transform trans, string childPath)
	{
		if ((object)trans == null)
		{
			Debug.LogError("Utils.findComp : trans is null");
			return default;
		}

		var find = trans.Find(childPath);
		if ((object)find == null)
		{
			Debug.LogError($"Utils.FindComp : not found transform child : ${trans.name} - ${childPath}");
			return default;
		}

		var comp = find.GetComponent<t>();
		if ((object)comp == null)
		{
			Debug.LogError($"Utils.FindComp : not found component : ${trans.name} - ${typeof(t)}");
			return default;
		}

		return comp;
	}
}
