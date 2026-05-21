// Сколько секунд ждать перед редиректом
let seconds = 3;

function startCountdown()
{
  const timerSpan = document.getElementById("timer");
  timerSpan.textContent = seconds;

  const interval = setInterval(() =>
  {
    seconds--;
    timerSpan.textContent = seconds;

    if (seconds <= 0)
    {
      clearInterval(interval);
      redirectBack();
    }
  }, 1000);
}


function redirectBack()
{
  if (document.referrer)
  {
    let url = new URL(document.referrer);

    // проверяем, что путь заканчивается именно на workplace.aspx (без учёта query)
    if (url.pathname.toLowerCase().includes("/workplace.aspx"))
    {
      let savedOper = localStorage.getItem("selectedPerson"); // достаём сохранённое значение

      if (savedOper) 
        url.searchParams.set("oper", savedOper);

      window.location.href = url.toString();
    }
    else
      window.location.href = document.referrer;   // загружаем страницу заново (не history.back!)
  }
  else
    window.location.href = '/workplace.aspx';     // если referrer пустой — отправляем на главную
}

window.onload = startCountdown;