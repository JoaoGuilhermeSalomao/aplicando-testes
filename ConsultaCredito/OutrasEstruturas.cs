namespace ConsultaCredito;

public enum StatusConsultaCredito
{
    Indefinido = -9,
    ParametroEnvioInvalido = -2,
    ErroComunicacao = -1,
    SemPendencias = 0,
    Inadimplente = 1
}

public class Pendencia
{
    public string Cpf { get; set; } = string.Empty;
    public string NomePessoa { get; set; } = string.Empty;
    public string NomeReclamante { get; set; } = string.Empty;
    public string DescricaoPendencia { get; set; } = string.Empty;
    public DateTime DataPendencia { get; set; }
    public double ValorPendencia { get; set; }
}
