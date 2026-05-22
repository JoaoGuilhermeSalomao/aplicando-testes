using FluentAssertions;
using Moq;
using Xunit;

namespace ConsultaCredito.Testes;

public class TestesAnaliseCredito
{
    private readonly Mock<IServicoConsultaCredito> _mock;

    private const string CpfInvalido = "123A";
    private const string CpfErroComunicacao = "76217486300";
    private const string CpfSemPendencias = "60487583752";
    private const string CpfInadimplente = "82226651209";

    public TestesAnaliseCredito()
    {
        _mock = new Mock<IServicoConsultaCredito>(MockBehavior.Strict);

        _mock.Setup(s => s.ConsultarPendenciasPorCpf(CpfInvalido))
            .Returns(() => null);

        _mock.Setup(s => s.ConsultarPendenciasPorCpf(CpfErroComunicacao))
            .Throws(new InvalidOperationException("Teste de erro de comunicacao"));

        _mock.Setup(s => s.ConsultarPendenciasPorCpf(CpfSemPendencias))
            .Returns(() => new List<Pendencia>());

        List<Pendencia> pendencias =
        [
            new Pendencia
            {
                Cpf = CpfInadimplente,
                NomePessoa = "Cliente Teste",
                NomeReclamante = "Empresas ACME",
                DescricaoPendencia = "Parcela nao paga",
                DataPendencia = new DateTime(2026, 5, 22),
                ValorPendencia = 900.50
            }
        ];

        _mock.Setup(s => s.ConsultarPendenciasPorCpf(CpfInadimplente))
            .Returns(() => pendencias);
    }

    [Fact]
    public void TestarCpfInvalidoComMoq()
    {
        StatusConsultaCredito status = ObterStatusAnaliseCredito(CpfInvalido);

        status.Should().Be(
            StatusConsultaCredito.ParametroEnvioInvalido,
            "um CPF invalido deve retornar parametro de envio invalido");
    }

    [Fact]
    public void TestarErroComunicacaoComMoq()
    {
        StatusConsultaCredito status = ObterStatusAnaliseCredito(CpfErroComunicacao);

        status.Should().Be(
            StatusConsultaCredito.ErroComunicacao,
            "falhas no servico externo devem retornar erro de comunicacao");
    }

    [Fact]
    public void TestarCpfSemPendenciasComMoq()
    {
        StatusConsultaCredito status = ObterStatusAnaliseCredito(CpfSemPendencias);

        status.Should().Be(
            StatusConsultaCredito.SemPendencias,
            "uma consulta sem registros deve retornar sem pendencias");
    }

    [Fact]
    public void TestarCpfInadimplenteComMoq()
    {
        StatusConsultaCredito status = ObterStatusAnaliseCredito(CpfInadimplente);

        status.Should().Be(
            StatusConsultaCredito.Inadimplente,
            "uma consulta com pendencias deve retornar inadimplente");
    }

    private StatusConsultaCredito ObterStatusAnaliseCredito(string cpf)
    {
        AnaliseCredito analise = new(_mock.Object);

        return analise.ConsultarSituacaoCpf(cpf);
    }
}
