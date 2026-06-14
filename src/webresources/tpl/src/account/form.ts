// Web resource de formulário da Account.
// Namespace evita poluir o escopo global (Xrm é global no D365).
// Nome no D365: tpl_/account/form.js  (o JS compilado é o que sobe).
namespace Tpl.Account {
  /**
   * Responsabilidade única: ao carregar o form da conta, ajustar a UI.
   * Referencie em Form Properties → OnLoad → Tpl.Account.onLoad.
   */
  export function onLoad(executionContext: Xrm.Events.EventContext): void {
    const formContext = executionContext.getFormContext();
    const nome = formContext.getAttribute("name")?.getValue();
    if (!nome) {
      formContext.ui.setFormNotification("Informe o nome da conta.", "INFO", "tpl_nome");
    }
  }
}
