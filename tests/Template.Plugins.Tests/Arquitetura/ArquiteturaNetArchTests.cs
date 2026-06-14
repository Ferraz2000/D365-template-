using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetArchTest.Rules;
using Template.Plugins.Common;
using Xunit;

namespace Template.Plugins.Tests
{
    /// <summary>
    /// Testes de arquitetura a **nível de IL** (NetArchTest/Mono.Cecil) — pegam dependências no
    /// corpo de método, que a reflection não vê. Pulam no Mono (rodam no CI Windows / .NET completo).
    /// </summary>
    public class ArquiteturaNetArchTests
    {
        private static readonly Assembly Assembly = typeof(PluginBase).Assembly;
        private static bool NoMono => Type.GetType("Mono.Runtime") != null;

        [Fact]
        public void Common_nao_depende_de_features()
        {
            if (NoMono) return; // NetArchTest não roda estável no Mono — validado no CI Windows
            var r = Types.InAssembly(Assembly).That()
                .ResideInNamespaceStartingWith("Template.Plugins.Common")
                .ShouldNot().HaveDependencyOnAny(
                    "Template.Plugins.Contas", "Template.Plugins.Contatos", "Template.Plugins.Integracao")
                .GetResult();
            Assert.True(r.IsSuccessful, Falhas(r));
        }

        [Fact]
        public void Integracao_nao_depende_de_features()
        {
            if (NoMono) return;
            var r = Types.InAssembly(Assembly).That()
                .ResideInNamespaceStartingWith("Template.Plugins.Integracao")
                .ShouldNot().HaveDependencyOnAny("Template.Plugins.Contas", "Template.Plugins.Contatos")
                .GetResult();
            Assert.True(r.IsSuccessful, Falhas(r));
        }

        [Fact]
        public void Contatos_nao_depende_de_Contas()
        {
            if (NoMono) return;
            var r = Types.InAssembly(Assembly).That()
                .ResideInNamespaceStartingWith("Template.Plugins.Contatos")
                .ShouldNot().HaveDependencyOn("Template.Plugins.Contas")
                .GetResult();
            Assert.True(r.IsSuccessful, Falhas(r));
        }

        private static string Falhas(TestResult r)
        {
            var nomes = r.FailingTypeNames ?? new List<string>();
            return "Violações: " + string.Join(", ", nomes);
        }
    }
}
