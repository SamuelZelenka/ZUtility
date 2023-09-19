public interface IQueryCreator
{
	(string key, string value)[] ToParameters();
}
