using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public List<Cell> cellList;

    [SerializeField] private InputController _inputController;
    [SerializeField] private TypeCellScriptable _typeCellScriptable;
    [SerializeField] private Transform backParent, cellParent;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        float screenHalfX = Screen.width / 2;
        screenHalfX = -screenHalfX;
        float screenHalfY = Screen.height / 2;

        float screenPartX = Screen.width / 7;
        float screenPartY = Screen.height / 12;

        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                Vector2 pos = new Vector2(screenHalfX + j * screenPartX + 80, screenHalfY - i * screenPartY - 500);

                GameObject back = new GameObject($"Back {i}.{j}");
                RectTransform backRect = back.AddComponent<RectTransform>();
                backRect.transform.SetParent(backParent.transform);
                backRect.anchoredPosition = pos;
                backRect.sizeDelta = new Vector2(130, 130);
                Image backImage = back.AddComponent<Image>();
                backImage.sprite = _typeCellScriptable._spriteBack;

                GameObject cell = new GameObject($"Cell {i * 7 + j}");
                Cell cellScript = cell.AddComponent<Cell>();
                cellList.Add(cellScript);
                cellScript.Init(_inputController);
                RectTransform backCell = cell.GetComponent<RectTransform>();
                backCell.transform.SetParent(cellParent.transform);
                backCell.anchoredPosition = pos;
                backCell.sizeDelta = new Vector2(90, 90);
            }
        }
    }

    public void MixElements()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                int rand = Random.Range(0, 3);
                cellList[i * 7 + j]._image.sprite = _typeCellScriptable._spriteModels[rand]._sprite;
                cellList[i * 7 + j]._typeCell = _typeCellScriptable._spriteModels[rand]._typeCell;
            }
        }

        _inputController.ActiveButton(false);
        CheckCellHorizontalVertical();
    }

    public void MixElements(List<Cell> cells)
    {
        for (int j = 0; j < cells.Count - 1; j++)
        {
            int rand = Random.Range(0, 3);
            cells[j]._image.sprite = _typeCellScriptable._spriteModels[rand]._sprite;
            cells[j]._typeCell = _typeCellScriptable._spriteModels[rand]._typeCell;
        }
    }

    public void CheckCellHorizontalVertical()
    {
        var allCells = new List<Cell>();
        var cells = new List<Cell>();

        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
                CheckCell(allCells, cells, i * 7 + j);

            if (cells.Count >= 3)
                allCells.AddRange(CheckDuplicate(cells));

            cells.Clear();
        }

        cells = new List<Cell>();
        for (int j = 0; j < 7; j++)
        {
            for (int i = 0; i < 7; i++)
                CheckCell(allCells, cells, i * 7 + j);

            if (cells.Count >= 3)
                allCells.AddRange(CheckDuplicate(cells));

            cells.Clear();
        }

        if (allCells.Count > 0)
            _inputController.MoveSizeCell(allCells);
        else _inputController.ActiveButton(true);
    }

    void CheckCell(List<Cell> allCells, List<Cell> cells, int index)
    {
        if (cells.Count == 0 || cellList[index]._typeCell == cells.Last()._typeCell)
            cells.Add(cellList[index]);
        else
        {
            if (cells.Count >= 3)
                allCells.AddRange(CheckDuplicate(cells));

            cells.Clear();
            cells.Add(cellList[index]);
        }
    }

    public bool CheckConnectCell(Cell cell)
    {
        var cells = new List<Cell>();

        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
                if (cells.Count == 0 || cellList[i * 7 + j]._typeCell == cells.Last()._typeCell)
                    cells.Add(cellList[i * 7 + j]);
                else
                {
                    if (cells.Count >= 3 && cells.Contains(cell))
                        return true;

                    cells.Clear();
                    cells.Add(cellList[i * 7 + j]);
                }

            if (cells.Count >= 3 && cells.Contains(cell))
                return true;

            cells.Clear();
        }

        for (int j = 0; j < 7; j++)
        {
            for (int i = 0; i < 7; i++)
                if (cells.Count == 0 || cellList[i * 7 + j]._typeCell == cells.Last()._typeCell)
                    cells.Add(cellList[i * 7 + j]);
                else
                {
                    if (cells.Count >= 3 && cells.Contains(cell))
                        return true;

                    cells.Clear();
                    cells.Add(cellList[i * 7 + j]);
                }

            if (cells.Count >= 3 && cells.Contains(cell))
                return true;

            cells.Clear();
        }

        return false;
    }

    List<Cell> CheckDuplicate(List<Cell> cells)
    {
        List<Cell> noDublicate = new List<Cell>();
        foreach (var i in cells)
            if (!noDublicate.Contains(i))
                noDublicate.Add(i);
        return noDublicate;
    }
}