using System;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using Template.Plugins.Common;
using Template.Plugins.Plugins.Account;
using Xunit;

namespace Template.Plugins.Tests.Plugins.Account
{
    public class AtualizarNomePluginTests
    {
        [Fact]
        public void Apara_espacos_do_nome_no_pre_operation()
        {
            // Arrange
            var ctx = new XrmFakedContext();
            var target = new Entity("account", Guid.NewGuid());
            target["name"] = "  Acme  ";

            var pluginCtx = ctx.GetDefaultPluginContext();
            pluginCtx.MessageName = Messages.Update;
            pluginCtx.Stage = Stages.PreOperation;
            pluginCtx.InputParameters = new ParameterCollection { { "Target", target } };

            // Act
            ctx.ExecutePluginWith<AtualizarNomePlugin>(pluginCtx);

            // Assert — Pre-Operation: alterar o Target basta
            Assert.Equal("Acme", target.GetAttributeValue<string>("name"));
        }

        [Fact]
        public void Ignora_quando_nao_ha_nome_no_target()
        {
            var ctx = new XrmFakedContext();
            var target = new Entity("account", Guid.NewGuid());

            var pluginCtx = ctx.GetDefaultPluginContext();
            pluginCtx.MessageName = Messages.Update;
            pluginCtx.Stage = Stages.PreOperation;
            pluginCtx.InputParameters = new ParameterCollection { { "Target", target } };

            var ex = Record.Exception(() => ctx.ExecutePluginWith<AtualizarNomePlugin>(pluginCtx));

            Assert.Null(ex);
            Assert.False(target.Contains("name"));
        }
    }
}
