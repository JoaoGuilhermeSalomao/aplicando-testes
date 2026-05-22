using System.Globalization;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace APIFinancas.Especificacoes;

[Binding]
public class CalculoJurosCompostosStepDefinition
{
    private static readonly CultureInfo CulturaPtBr = new("pt-BR");

    private double _valorEmprestimo;
    private int _numMeses;
    private double _percTaxa;
    private double _valorCalculado;

    [Given(@"que o valor do emprestimo e de R\$ (.*)")]
    public void PreencherValorEmprestimo(string valorEmprestimo)
    {
        _valorEmprestimo = ConverterDecimalPtBr(valorEmprestimo);
    }

    [Given(@"que este emprestimo sera por (.*) meses")]
    public void PreencherNumeroMeses(int numMeses)
    {
        _numMeses = numMeses;
    }

    [Given(@"que a taxa de juros e de (.*)% ao mes")]
    public void PreencherPercentualTaxa(string percTaxa)
    {
        _percTaxa = ConverterDecimalPtBr(percTaxa);
    }

    [When(@"eu solicitar o calculo do valor total a ser pago ao final do periodo")]
    public void ProcessarCalculoJurosCompostos()
    {
        _valorCalculado = CalculoFinanceiro.CalcularValorComJurosCompostos(
            _valorEmprestimo,
            _numMeses,
            _percTaxa);
    }

    [Then(@"o resultado sera (.*)")]
    public void ValidarResultado(string valorFinalEmprestimo)
    {
        double valorEsperado = ConverterDecimalPtBr(valorFinalEmprestimo);

        _valorCalculado.Should().Be(
            valorEsperado,
            $"valorEmprestimo: {_valorEmprestimo}; numMeses: {_numMeses}; percTaxa: {_percTaxa}");
    }

    private static double ConverterDecimalPtBr(string valor)
    {
        return double.Parse(valor, NumberStyles.Number, CulturaPtBr);
    }
}
