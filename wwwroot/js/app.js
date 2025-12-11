const apiBase = ""; // mismo origen

const $ = (id) => document.getElementById(id);

// --- Clasificar intención ---
const btnIntencion = $("btn-intencion");
const txtIntencion = $("input-intencion");
const statusIntencion = $("status-intencion");
const resultIntencion = $("result-intencion");

btnIntencion.addEventListener("click", async () => {
  const inputUsuario = txtIntencion.value.trim();

  if (!inputUsuario) {
    statusIntencion.textContent = "Ingrese un texto.";
    statusIntencion.className = "status error";
    return;
  }

  btnIntencion.disabled = true;
  statusIntencion.textContent = "Llamando a la API...";
  statusIntencion.className = "status";

  try {
    const response = await fetch("/api/intencion/clasificar", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ inputUsuario })
    });

    const data = await response.json();

    statusIntencion.textContent = data.isSuccess ? "OK" : "Error";
    statusIntencion.className = data.isSuccess ? "status ok" : "status error";

    resultIntencion.textContent = JSON.stringify(data, null, 2);
  } catch (err) {
    statusIntencion.textContent = "Error: " + err.message;
    statusIntencion.className = "status error";
  } finally {
    btnIntencion.disabled = false;
  }
});

// --- Resumir texto ---
const btnResumen = $("btn-resumen");
const txtResumen = $("input-resumen");
const statusResumen = $("status-resumen");
const resultResumen = $("result-resumen");

btnResumen.addEventListener("click", async () => {
  const texto = txtResumen.value.trim();

  if (!texto) {
    statusResumen.textContent = "Ingrese un texto.";
    statusResumen.className = "status error";
    return;
  }

  btnResumen.disabled = true;
  statusResumen.textContent = "Llamando a la API...";
  statusResumen.className = "status";

  try {
    const response = await fetch("/api/texto/resumir", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ texto })
    });

    const data = await response.json();

    statusResumen.textContent = data.isSuccess ? "OK" : "Error";
    statusResumen.className = data.isSuccess ? "status ok" : "status error";

    resultResumen.textContent = JSON.stringify(data, null, 2);
  } catch (err) {
    statusResumen.textContent = "Error: " + err.message;
    statusResumen.className = "status error";
  } finally {
    btnResumen.disabled = false;
  }
});
