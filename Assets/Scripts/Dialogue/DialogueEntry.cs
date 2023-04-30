using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEntry
{
    public string nameText;
    public string lineText;
    public int spriteIndex;

    public DialogueEntry(string _nameText, string _lineText, int _spriteIndex) 
    {
            this.nameText = _nameText;
            this.lineText = _lineText;
            this.spriteIndex = _spriteIndex;
    }
}
