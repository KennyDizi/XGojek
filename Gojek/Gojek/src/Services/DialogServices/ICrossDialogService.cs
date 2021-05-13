namespace Gojek.Services.DialogServices
{
    public interface ICrossDialogService
    {
        ICrossDialogProvider Dialoger { get; set; }
    }
}