import { XrmMockGenerator } from "xrm-mock";
import { onLoad, onChangeNome, onSave } from "./form";

/** Monta um SaveEventContext sobre o formContext do xrm-mock (eventArgs é manual). */
function saveContext(): { ctx: Xrm.Events.SaveEventContext; preventDefault: jest.Mock } {
  const formContext = XrmMockGenerator.getEventContext().getFormContext();
  const preventDefault = jest.fn();
  const ctx = {
    getFormContext: () => formContext,
    getEventArgs: () => ({ preventDefault, isDefaultPrevented: () => false }),
  } as unknown as Xrm.Events.SaveEventContext;
  return { ctx, preventDefault };
}

describe("Account form (xrm-mock)", () => {
  beforeEach(() => XrmMockGenerator.initialise());

  it("onLoad normaliza o nome", () => {
    const attr = XrmMockGenerator.Attribute.createString("name", "  Acme  ");
    onLoad(XrmMockGenerator.getEventContext());
    expect(attr.getValue()).toBe("Acme");
  });

  it("onLoad avisa quando o nome está vazio", () => {
    XrmMockGenerator.Attribute.createString("name", "");
    const ctx = XrmMockGenerator.getEventContext();
    const spy = jest.spyOn(ctx.getFormContext().ui, "setFormNotification");
    onLoad(ctx);
    expect(spy).toHaveBeenCalled();
  });

  it("onChangeNome colapsa espaços internos", () => {
    const attr = XrmMockGenerator.Attribute.createString("name", "Conta   X");
    onChangeNome(XrmMockGenerator.getEventContext());
    expect(attr.getValue()).toBe("Conta X");
  });

  it("onSave cancela o salvamento quando o nome está vazio", () => {
    XrmMockGenerator.Attribute.createString("name", "   ");
    const { ctx, preventDefault } = saveContext();
    onSave(ctx);
    expect(preventDefault).toHaveBeenCalled();
  });

  it("onSave permite salvar quando há nome", () => {
    XrmMockGenerator.Attribute.createString("name", "Acme");
    const { ctx, preventDefault } = saveContext();
    onSave(ctx);
    expect(preventDefault).not.toHaveBeenCalled();
  });
});
