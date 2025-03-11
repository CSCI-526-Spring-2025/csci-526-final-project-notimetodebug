export async function fetchData() {
  let response = await fetch(
    "https://game-telemetry-default-rtdb.firebaseio.com/telemetry_sodifaposid93roj94ek9923oij.json"
  );

  return await response.json();
}
