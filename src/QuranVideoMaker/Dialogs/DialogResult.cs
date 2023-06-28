namespace QuranVideoMaker.Dialogs
{
    public class DialogResult
    {
        public bool? Result { get; set; }

        public object Data { get; set; }

        public DialogResult(bool? result, object data)
        {
            Result = result;
            Data = data;
        }
    }
}
