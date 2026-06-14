import { XrmMockGenerator } from "xrm-mock";
import { onLoad } from "./form";

describe("Account onLoad (xrm-mock)", () => {
  beforeEach(() => XrmMockGenerator.initialise());

  it("normaliza o nome quando há valor", () => {
    const attr = XrmMockGenerator.Attribute.createString("name", "  Acme  ");

    onLoad(XrmMockGenerator.getEventContext());

    expect(attr.getValue()).toBe("Acme");
  });

  it("avisa quando o nome está vazio", () => {
    XrmMockGenerator.Attribute.createString("name", "");
    const context = XrmMockGenerator.getEventContext();
    const spy = jest.spyOn(context.getFormContext().ui, "setFormNotification");

    onLoad(context);

    expect(spy).toHaveBeenCalled();
  });
});
