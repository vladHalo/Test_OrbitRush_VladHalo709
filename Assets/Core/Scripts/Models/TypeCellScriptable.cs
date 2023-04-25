using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TypeCell", menuName = "ScriptableObjects/TypeCell")]
public class TypeCellScriptable : ScriptableObject
{
    public Sprite _spriteBack;
    public List<SpriteModel> _spriteModels;
}