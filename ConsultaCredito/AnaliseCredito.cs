namespace ConsultaCredito;

public class AnaliseCredito
{
    private readonly IServicoConsultaCredito _servicoConsultaCredito;

    public AnaliseCredito(IServicoConsultaCredito servicoConsultaCredito)
    {
        _servicoConsultaCredito = servicoConsultaCredito;
    }

    public StatusConsultaCredito ConsultarSituacaoCpf(string cpf)
    {
        try
        {
            IList<Pendencia>? pendencias =
                _servicoConsultaCredito.ConsultarPendenciasPorCpf(cpf);

            if (pendencias is null)
            {
                return StatusConsultaCredito.ParametroEnvioInvalido;
            }

            if (pendencias.Count == 0)
            {
                return StatusConsultaCredito.SemPendencias;
            }

            return StatusConsultaCredito.Inadimplente;
        }
        catch
        {
            return StatusConsultaCredito.ErroComunicacao;
        }
    }
}
