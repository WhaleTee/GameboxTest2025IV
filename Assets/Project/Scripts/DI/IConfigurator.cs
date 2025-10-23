public interface IConfigurator<in C, out T> {
  T Configure(C context);
}