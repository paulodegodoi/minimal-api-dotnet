namespace MinimalAPI.Estudandes;

public class Estudante
{
    public Guid Id { get; init; }
    public string Nome { get; private set; }
    public bool Ativo { get; private set; }

    public Estudante(string nome)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Ativo = true;
    }

    public void UpdateNome(string nome)
    {
        Nome = nome;
    }

    public void Disable()
    {
        Ativo = false;
    }
}