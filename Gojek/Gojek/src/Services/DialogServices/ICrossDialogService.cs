namespace Gojek.src.Services.DialogServices
{
    public interface ICrossDialogService
    {
        ICrossDialogProvider Dialoger { get; set; }
    }
}