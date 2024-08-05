using System.Numerics;
using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeNumericField<TNumber> : TeEditorBase
    where TNumber : INumber<TNumber>, IMinMaxValue<TNumber>
{
    [Parameter] public ITeNumericFieldOption NumericFieldOption { get; set; } = default!;
    
    TeNumericFieldProperty<TNumber> property = default!;

    TNumber numberValue = TNumber.Zero;
    protected override void OnInitialized()
    {
        if (Data != null)
        {
            var converted = NumericFieldOption.Converter.ToField(Data);
            if (converted is TNumber numberValue)
            {
                this.numberValue = numberValue;
            }
        }

        property = GetNumberFieldProperty();
    }

    private async Task OnValueChanged(TNumber value)
    {
        numberValue = value;
        Data = NumericFieldOption.Converter.FromField(value);
        await DataChanged.InvokeAsync(Data);
    }

    private async Task<IEnumerable<string>> NumericFieldValidation(TNumber value)
    {
        var data = NumericFieldOption.Converter.FromField(value);
        var errors = new List<string>();
        if (NumericFieldOption?.Validations != null && data != null)
        {
            // Do not use Task.WhenAll
            foreach (var validation in NumericFieldOption.Validations)
            {
                try
                {
                    if (!await validation.Func(data))
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

    private TeNumericFieldProperty<TNumber> GetNumberFieldProperty()
    {
        if (NumericFieldOption.Property is TeNumericFieldProperty<TNumber> property)
        {
            return property;
        }
        else
        {
            return new TeNumericFieldProperty<TNumber>();
        }
    }
}
