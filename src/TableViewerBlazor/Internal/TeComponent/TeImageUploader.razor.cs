using Microsoft.AspNetCore.Components.Forms;
using TableViewerBlazor.Options;

namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeImageUploader : TeEditorBase
{
    [Parameter] public ITeImageUploaderOption ImageUploaderOption { get; set; } = default!;
    private TvButtonStyle Style => ImageUploaderOption.ButtonStyle;
    private async Task LoadImage(IBrowserFile file)
    {
        if (ImageUploaderOption is TeBase64ImageUploaderOption option64)
        {
            using var stream = file.OpenReadStream(ImageUploaderOption.MaxAllowedSize);
            var bytes = new byte[stream.Length];
            await stream.ReadAsync(bytes, 0, bytes.Length);
            var base64 = Convert.ToBase64String(bytes);
            await DataChanged.InvokeAsync(base64);
        }
        else if (ImageUploaderOption is TeFilePathImageUploaderOption optionFile)
        {
            var filePath = await optionFile.SaveFileAsync(file);
            await DataChanged.InvokeAsync(filePath);
        }
    }

    private async Task<IEnumerable<string>> ImageValidation(object value)
    {
        var errors = new List<string>();
        if (ImageUploaderOption?.Validations != null)
        {
            // Do not use Task.WhenAll
            foreach (var validation in ImageUploaderOption.Validations)
            {
                try
                {
                    if (!await validation.Func(value!))
                    {
                        errors.Add(validation.Message);
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(ex.Message);
                }
            }
        }
        return errors;
    }
}
