document.addEventListener("DOMContentLoaded", function () {

  OperatorList.init(
    "ddListPerson",
    "ddListBrigadier",
    "./Java/Data/teamconfig.json"
  );

});


const OperatorList = (function () {

  const originalOptions = [];

  let allGroups = [];
  let currentGroup = null;

  let ddOperator = null;
  let ddBrigadier = null;

  const brigadierMap = new Map();
  const operatorMap = new Map();

  async function init(operatorSelectId, brigadierSelectId, dataUrl) {

    ddOperator = document.getElementById(operatorSelectId);
    ddBrigadier = document.getElementById(brigadierSelectId);

    ddBrigadier.addEventListener("change", onBrigadierChange);

    buildOperatorIndex();

    await loadData(dataUrl);

    // если бригадир уже выбран при загрузке
    if (ddBrigadier.value && ddBrigadier.value !== "0") {
      onBrigadierChange();
    }
  }

  async function loadData(url) {

    try {

      const res = await fetch(url, {
        cache: "no-cache"
      });

      if (!res.ok) {
        throw new Error(`HTTP error: ${res.status}`);
      }

      allGroups = await res.json();

      console.log("JSON loaded:", allGroups);

      buildBrigadierMap();
    }
    catch (e) {
      console.error("LOAD DATA ERROR:", e);
    }
  }

  function buildBrigadierMap() {

    brigadierMap.clear();

    for (const group of allGroups) {
      brigadierMap.set(group.idMainOper, group.items || []);
    }
  }

  function buildOperatorIndex() {

    operatorMap.clear();
    originalOptions.length = 0;

    const opts = ddOperator.options;

    for (let i = 0; i < opts.length; i++) {

      const opt = opts[i];

      const key = normalizeName(opt.text);

      operatorMap.set(key, opt);

      originalOptions.push({
        value: opt.value,
        text: opt.text,
        idSheduleOperator: opt.getAttribute("idsheduleoperator")
      });
    }
  }

  function normalizeName(fio) {

    return fio
      .trim()
      .toLowerCase()
      .split(/\s+/)
      .slice(0, 2)
      .join(" ");
  }

  function onBrigadierChange() {

    const brigadierId = parseInt(ddBrigadier.value);

    currentGroup = brigadierMap.get(brigadierId);

    // ничего не выбрано → показать всех
    if (!currentGroup) {
      renderOperators(originalOptions);
      return;
    }

    const allowedMap = new Map();

    for (const item of currentGroup) {

      const key = normalizeName(item.fio);

      const opt = operatorMap.get(key);

      if (!opt) {
        console.warn(`${key} not found in operator list`);
        continue;
      }

      allowedMap.set(key, {
        value: opt.value,
        text: opt.text,
        idSheduleOperator: opt.getAttribute("idsheduleoperator"),
        coef: item.coef
      });

      if (!operatorMap.has(key)) {
        console.warn(`${key} not found in operator list`);
      }
    }

    const filtered = Array.from(allowedMap.values());

    renderOperators(filtered);
  }

  function renderOperators(list) {

    const selectedValue = ddOperator.value;

    ddOperator.innerHTML = "";

    for (const item of list) {

      const opt = document.createElement("option");

      opt.value = item.value;
      opt.text = item.text;

      if (item.idSheduleOperator != null) {
        opt.setAttribute("idsheduleoperator", item.idSheduleOperator);
        opt.setAttribute("data-coef", item.coef ?? 0);
      }

      ddOperator.appendChild(opt);
    }

    // восстановить выбранное значение если возможно
    if (selectedValue) {

      const exists = [...ddOperator.options]
        .some(x => x.value === selectedValue);

      if (exists) {
        ddOperator.value = selectedValue;
      }
    }
  }

  function getCurrentOperatorCoef() {

    if (!currentGroup)
      return 1;

    const selectedText =
      ddOperator.options[ddOperator.selectedIndex]?.text;

    if (!selectedText)
      return 1;

    const key = normalizeName(selectedText);

    const found = currentGroup.find(x => {
      return normalizeName(x.fio) === key;
    });

    return found ? found.coef : 1;
  }

  function getCurrentOperatorInfo() {

    if (!currentGroup)
      return null;

    const selectedText =
      ddOperator.options[ddOperator.selectedIndex]?.text;

    if (!selectedText)
      return null;

    const key = normalizeName(selectedText);

    return currentGroup.find(x => {
      return normalizeName(x.fio) === key;
    }) || null;
  }

  return {
    init,
    getCurrentOperatorInfo
  };

})();