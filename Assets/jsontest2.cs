using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class jsontest2 : MonoBehaviour
{

    public prog kprog;

    void Start()
    {
        kprog = new prog();

        action1 ka0 = new action1(1);
        action1 ka1 = new action1(2);
        action2 ka2 = new action2(3, "yo");
        action1 ka3 = new action1(-1);

        kprog.nodes = new node[] { ka0, ka1, ka2, ka3 };



        string prog = JsonUtility.ToJson(kprog, true);
        print(prog);
        StreamWriter writer = new StreamWriter("Assets/Resources/test2.txt");
        writer.WriteLine(prog);
        writer.Close();
        print("done");
    }

}


[Serializable]
public class prog
{
    [SerializeReference]
    public node[] nodes;
}

[Serializable]
public class node
{
    public int nextNodeIndex;
}

public class action1 : node
{
    public action1(int i)
    {
        nextNodeIndex = i;
    }
}

public class action2 : node
{
    public string str;
    public action2(int i, string s)
    {
        nextNodeIndex = i;
        str = s;
    }
}