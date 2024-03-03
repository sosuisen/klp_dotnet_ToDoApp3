using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ToDoApp3;

namespace ToDoApp3
{
    internal partial class ToDoViewModel : ObservableObject
    {
        private ToDoModel _model;

        public ObservableCollection<ToDo> ListViewRows { get; set; }

        [ObservableProperty]
        private string _newToDoName = "";

        [ObservableProperty]
        private DateTime _newToDoDeadline = DateTime.Today;

        [ObservableProperty]
        private int _newToDoPriority = 1;

        public ToDoViewModel()
        {
            _model = new();
            ListViewRows = new(_model.ToDos);
            foreach (var todo in ListViewRows)
            {
                todo.PropertyChanged += ToDoPropertyChanged;
            }
        }

        private void ToDoPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is ToDo todo)
            {
                switch (e.PropertyName)
                {
                    case nameof(ToDo.Name):
                        _model.UpdateName(todo, todo.Name);
                        break;
                    case nameof(ToDo.Deadline):
                        _model.UpdateDeadline(todo, todo.Deadline);
                        break;
                    case nameof(ToDo.Completed):
                        _model.UpdateCompleted(todo, todo.Completed);
                        break;
                    case nameof(ToDo.Priority):
                        _model.UpdatePriority(todo, todo.Priority);
                        break;
                }
            }
        }

        [RelayCommand]
        private void AddToDo()
        {
            var todo = new ToDo(
                name: NewToDoName,
                deadline: NewToDoDeadline,
                priority: NewToDoPriority
                );
            var newToDo = _model.Add(todo);
            if (newToDo != null)
            {
                newToDo.PropertyChanged += ToDoPropertyChanged;
                ListViewRows.Add(newToDo);
            }
            NewToDoName = "";
        }

        [RelayCommand]
        private void DeleteToDo(object parameter)
        {
            if (parameter is ToDo item)
            {
                ListViewRows.Remove(item);
                _model.Delete(item);
            }
        }
    }
}
