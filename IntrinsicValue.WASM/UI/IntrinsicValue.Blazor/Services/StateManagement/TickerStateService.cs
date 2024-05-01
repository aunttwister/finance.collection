using IntrinsicValue.Blazor.Model;

namespace IntrinsicValue.Blazor.Services.StateManagement
{
    public class TickerStateService
    {
        private TickerDto _selectedTicker;
        public TickerDto SelectedTicker
        {
            get => _selectedTicker;
            set
            {
                _selectedTicker = value;
                NotifyStateChanged();
            }
        }

        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

}
