// Helpers compartilhados entre web resources (reuso, sem repetir).
namespace Tpl.Common {
  export function info(formContext: Xrm.FormContext, message: string, id: string): void {
    formContext.ui.setFormNotification(message, "INFO", id);
  }

  export function clear(formContext: Xrm.FormContext, id: string): void {
    formContext.ui.clearFormNotification(id);
  }
}
