namespace TableViewerBlazor.Options;

public interface ITeImageUploaderOption
{
    IEnumerable<ITeValidation> Validations { get; }
}

public class TeImageUploaderOption : ITeImageUploaderOption
{

}