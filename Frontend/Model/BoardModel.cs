﻿using Frontend.Controllers;
using Frontend.Utils;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Collections.ObjectModel;

namespace Frontend.Model
{
    public class BoardModel : NotifiableModelObject<BoardController>
    {
        private bool _isExpanded;

        public string Name { get; init; }
        public string Owner { get; set; }
        public ObservableCollection<string> Members { get; init; }
        public ObservableCollection<ColumnModel> Columns { get; init; }
        public bool IsExpanded { get => _isExpanded; set { _isExpanded = value; RaisePropertyChanged(nameof(IsExpanded)); } }

        public BoardModel(BoardController controller, BoardSL board) : base(controller)
        {
            Name = board.Name;
            Owner = board.Owner;
            Members = new(board.Members);
            Columns = new(board.Columns.Select(c => new ColumnModel(c)));
            IsExpanded = false;
        }
    }
}