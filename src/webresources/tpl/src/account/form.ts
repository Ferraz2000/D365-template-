import { normalizeName } from "../format";

const NOTIF_NOME = "tpl_nome";

/**
 * Handlers do formulário de Account. Finos: delegam a regra ao módulo puro `format`.
 * Bundle gera o global `Tpl`. Registre em Form Properties:
 *   - OnLoad            → Tpl.onLoad
 *   - OnChange (name)   → Tpl.onChangeNome
 *   - OnSave            → Tpl.onSave   (marque "Pass execution context as first parameter")
 */
export function onLoad(executionContext: Xrm.Events.EventContext): void {
  aplicarNome(executionContext.getFormContext());
}

export function onChangeNome(executionContext: Xrm.Events.EventContext): void {
  aplicarNome(executionContext.getFormContext());
}

/** OnSave: valida e **cancela o save** (preventDefault) se o nome estiver vazio. */
export function onSave(executionContext: Xrm.Events.SaveEventContext): void {
  const formContext = executionContext.getFormContext();
  const attr = formContext.getAttribute("name") as Xrm.Attributes.StringAttribute;
  const nome = normalizeName(attr?.getValue());
  if (!nome) {
    formContext.ui.setFormNotification("O nome da conta é obrigatório.", "ERROR", NOTIF_NOME);
    executionContext.getEventArgs().preventDefault(); // impede o salvamento
  }
}

function aplicarNome(formContext: Xrm.FormContext): void {
  const attr = formContext.getAttribute("name") as Xrm.Attributes.StringAttribute;
  const nome = normalizeName(attr?.getValue());
  if (!nome) {
    formContext.ui.setFormNotification("Informe o nome da conta.", "INFO", NOTIF_NOME);
    return;
  }
  formContext.ui.clearFormNotification(NOTIF_NOME);
  attr.setValue(nome);
}
