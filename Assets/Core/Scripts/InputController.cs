using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class InputController : MonoBehaviour
{
    [SerializeField] private Board _board;
    [SerializeField] private List<int> _selectedIndex;
    [SerializeField] private float _speed;

    private UIController _uiController;

    void Start()
    {
        _uiController = FindObjectOfType<UIController>();
        _selectedIndex = new List<int>();
    }

    public void SelectButton(Cell cellSelected)
    {
        Debug.Log(cellSelected.name);
        _selectedIndex.Add(_board.cellList.IndexOf(cellSelected));

        if (_selectedIndex.Count < 2) return;

        if (_selectedIndex[0] == _selectedIndex[1]
            || _selectedIndex[0] - 1 != _selectedIndex[1]
            && _selectedIndex[0] + 1 != _selectedIndex[1]
            && _selectedIndex[0] - 7 != _selectedIndex[1]
            && _selectedIndex[0] + 7 != _selectedIndex[1]
            || (_selectedIndex[0] + 1) % 7 == 0 && _selectedIndex[0] + 1 == _selectedIndex[1]
            || (_selectedIndex[1] + 1) % 7 == 0 && _selectedIndex[1] + 1 == _selectedIndex[0]
        )
        {
            _selectedIndex.Clear();
            return;
        }

        Cell cell = _board.cellList[_selectedIndex[0]];
        _board.cellList[_selectedIndex[0]] = _board.cellList[_selectedIndex[1]];
        _board.cellList[_selectedIndex[1]] = cell;

        bool isTrueMove = _board.CheckConnectCell(_board.cellList[_selectedIndex[0]]);
        if (!isTrueMove) isTrueMove = _board.CheckConnectCell(_board.cellList[_selectedIndex[1]]);

        if (!isTrueMove)
        {
            cell = _board.cellList[_selectedIndex[0]];
            _board.cellList[_selectedIndex[0]] = _board.cellList[_selectedIndex[1]];
            _board.cellList[_selectedIndex[1]] = cell;
        }

        MoveCell(_board.cellList[_selectedIndex[0]], _board.cellList[_selectedIndex[1]],
            isTrueMove);

        ActiveButton(false);
        _selectedIndex.Clear();
    }

    void MoveCell(Cell oneCell, Cell twoCell, bool isTrueMove)
    {
        Vector2 posOne = oneCell.transform.position;
        Vector2 posTwo = twoCell.transform.position;

        oneCell.transform.DOMove(posTwo, _speed).SetEase(Ease.Linear);
        twoCell.transform.DOMove(posOne, _speed).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (isTrueMove)
            {
                ActiveButton(true);
                _board.CheckCellHorizontalVertical();
            }
        });

        if (!isTrueMove)
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Insert(_speed, oneCell.transform.DOMove(posOne, _speed).SetEase(Ease.Linear));
            mySequence.Insert(_speed,
                twoCell.transform.DOMove(posTwo, _speed).SetEase(Ease.Linear)
                    .OnComplete(() => { ActiveButton(true); }));
        }
    }

    public void ActiveButton(bool isEnable) => _board.cellList.ForEach(x => x._button.enabled = isEnable);

    public void MoveSizeCell(List<Cell> cells)
    {
        List<Cell> newCells = new List<Cell>(cells);
        ActiveButton(false);
        newCells.ForEach(x => x.transform.DOScale(Vector3.zero, _speed));
        newCells.Last().transform.DOScale(Vector3.zero, _speed).OnComplete(() =>
        {
            _uiController.ChangeScore(newCells.Count);
            _board.MixElements(newCells);
        });

        Sequence mySequence = DOTween.Sequence();
        newCells.ForEach(x => mySequence.Insert(_speed, x.transform.DOScale(Vector3.one, _speed)));
        mySequence.Insert(_speed, newCells.Last().transform.DOScale(Vector3.one, _speed).OnComplete(
            () => { _board.CheckCellHorizontalVertical(); }));
    }
}