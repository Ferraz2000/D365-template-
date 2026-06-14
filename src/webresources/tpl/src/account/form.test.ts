import { onLoad } from "./form";

function fakeContext(name: string | null) {
  const setValue = jest.fn();
  const setFormNotification = jest.fn();
  const attr = { getValue: () => name, setValue };
  const formContext = {
    getAttribute: () => attr,
    ui: { setFormNotification },
  };
  return {
    executionContext: { getFormContext: () => formContext } as unknown as Xrm.Events.EventContext,
    setValue,
    setFormNotification,
  };
}

describe("Account onLoad", () => {
  it("avisa quando o nome está vazio", () => {
    const { executionContext, setFormNotification } = fakeContext("");
    onLoad(executionContext);
    expect(setFormNotification).toHaveBeenCalled();
  });

  it("normaliza o nome quando há valor", () => {
    const { executionContext, setValue } = fakeContext("  Acme  ");
    onLoad(executionContext);
    expect(setValue).toHaveBeenCalledWith("Acme");
  });
});
