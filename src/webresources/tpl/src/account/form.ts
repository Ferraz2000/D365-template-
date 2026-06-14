import { normalizeName } from "../format";

/**
 * OnLoad do formulário de Account. Fino: delega a regra ao módulo puro `format`.
 * Bundle gera o global `Tpl` → registre em Form Properties → OnLoad → `Tpl.onLoad`.
 */
export function onLoad(executionContext: Xrm.Events.EventContext): void {
  const formContext = executionContext.getFormContext();
  const attr = formContext.getAttribute("name") as Xrm.Attributes.StringAttribute;
  const nome = normalizeName(attr?.getValue());

  if (!nome) {
    formContext.ui.setFormNotification("Informe o nome da conta.", "INFO", "tpl_nome");
    return;
  }
  attr.setValue(nome);
}
