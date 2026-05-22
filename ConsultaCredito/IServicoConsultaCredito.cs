namespace ConsultaCredito;

public interface IServicoConsultaCredito
{
    IList<Pendencia>? ConsultarPendenciasPorCpf(string cpf);
}
