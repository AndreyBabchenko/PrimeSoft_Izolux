window.addEventListener("DOMContentLoaded", function () {

  // Если шпросы
  if (!sheetData.info.bShpros)
    return;

  const sheet = document.querySelector(".sheet");
  if (sheet) sheet.classList.remove("hidden");

  LoadMainPlotData(); // общие данные
  LoadFramesData();   // Данные о рамке
  LoadShprosInfoData(); // Общие данные о шпросах
  LoadShprosData();     // Данные о шпросах
 
});

function LoadMainPlotData()
{
  document.getElementById("cellClient").innerText = sheetData.info.ClientName;
  document.getElementById("cellDateComplite").innerText = sheetData.info.DateComplite.split(" ")[0];;
  document.getElementById("cellAccountNum").innerText = sheetData.info.AccountNum;
  document.getElementById("cellPosNum").innerText = sheetData.info.PosNum;
  document.getElementById("cellCountGP").innerText = sheetData.info.nCountGP;
  document.getElementById("cellSize").innerText = sheetData.info.Size;
  document.getElementById("cellFrame").innerText = sheetData.info.Frame;
  document.getElementById("cellFrameCount").innerText = sheetData.info.FrameCount;
  document.getElementById("cellGas").innerText = sheetData.info.Gas;
  document.getElementById("cellFormula").innerText = sheetData.info.Formula;
  document.getElementById("cellLayout").innerText = sheetData.info.Layout;
}


function LoadFramesData() {

  if (!window.sheetData || !window.sheetData.frames) {
    console.warn("Нет данных frames");
    return;
  }

  const table = document.getElementById("sizeFrameTable");
  if (!table) {
    console.error("Элемент sizeFrameTable не найден");
    return;
  }

  let html = "";

  window.sheetData.frames.forEach(row => {
    html += `
            <div class="cell">${row.num}</div>
            <div class="cell">${row.leng}</div>
            <div class="cell">${row.lengReal}</div>
            <div class="cell">${row.R}</div>
            <div class="cell">${row.R_Real}</div>
        `;
  });

  table.insertAdjacentHTML("beforeend", html);
}

function LoadShprosInfoData() {
  document.getElementById("ColorOut").innerText     = sheetData.shprossInfo.ColorName;
  document.getElementById("ColorIn").innerText      = sheetData.shprossInfo.ColorInsName;
  document.getElementById("PlotWidth").innerText    = sheetData.shprossInfo.WidthName;
  document.getElementById("Manufacturer").innerText = sheetData.shprossInfo.Manufacter;
  document.getElementById("CamNum").innerText       = sheetData.shprossInfo.CameraShprosName;
}


function LoadShprosData() {

  if (!window.sheetData || !window.sheetData.shpros) {
    console.warn("Нет данных shpros");
    return;
  }

  const table = document.getElementById("sizeShprosTable");
  if (!table) {
    console.error("Элемент sizeShprosTable не найден");
    return;
  }

  let html = "";

  window.sheetData.shpros.forEach(row => {
    html += `
            <div class="cell">${row.MarkIdentic}</div>
            <div class="cell">${row.Count}</div>
            <div class="cell">${row.LengReal}</div>
            <div class="cell">${row.AngleLef}</div>
            <div class="cell">${row.AngleRig}</div>
        `;
  });

  table.insertAdjacentHTML("beforeend", html);
}