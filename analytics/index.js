import { fetchData } from "./fetchData.js";

let data;
let selectedSessionURL = undefined;
let damageStats;

export async function init() {
  data = Object.values(await fetchData());
  let builds = getAvailableBuilds(data);

  setupBuildSelector(builds);

  damageStats = calculateDamageStats(data);
}

export function changeBuild(value) {
  if (value == "None") {
    selectedSessionURL = undefined;
  } else {
    selectedSessionURL = `https://csci-526-spring-2025.github.io/csci-526-final-project-notimetodebug/${value}`;
  }
  damageStats = calculateDamageStats(data, selectedSessionURL);
  drawDamageStatsChart(damageStats);
  console.log(damageStats);
}

function drawDamageStatsChart(data) {
  const ctx = document.getElementById("damage-chart-display");

  let labels = ["Spikes", "Player:Bullet", "Enemy:Bullet", "Enemy Touch"];

  let chartData = {
    labels: labels,
    datasets: Object.keys(data).map((key) => {
      let enemyBulletHits = Object.keys(data[key])
        .filter((k) => k.startsWith("GroundEnemy"))
        .reduce((p, c, i, a) => {
          p += data[key][c];
        }, 0);

      return {
        label: key,
        data: [
          data[key]["Spikes"],
          data[key]["Player:Bullet"],
          enemyBulletHits,
          data[key]["Enemy Touch"],
        ],
      };
    }),
  };

  new Chart(ctx, {
    type: "bar",
    data: chartData,
    options: {
      plugins: {
        title: {
          display: true,
          text: "Damage Distribution",
        },
      },
      responsive: true,
      scales: {
        x: {
          stacked: true,
        },
        y: {
          stacked: true,
        },
      },
    },
  });
}

function calculateDamageStats(data, build) {
  return data
    .filter((ev) => ev.eventName == "PLAYER_DAMAGED" && ev.sessionURL == build)
    .reduce((p, c, i, a) => {
      if (!p[c.levelName]) p[c.levelName] = {};
      if (!p[c.levelName][c.eventData]) p[c.levelName][c.eventData] = 0;

      p[c.levelName][c.eventData] += 1;

      return p;
    }, []);
}

function setupBuildSelector(items) {
  let buildSelector = document.getElementById("build-selector");

  buildSelector.addEventListener("change", (ev) => {
    changeBuild(ev.target.value);
  });

  buildSelector.innerHTML += `
        ${items
          .map((item) => {
            return `<option name="${item}" value="${item}">${item}</option>`;
          })
          .join("")}
    `;
}

function getAvailableBuilds(data) {
  let builds = [
    ...new Set(data.map((item) => extractBuildName(item.sessionURL))),
  ];
  return builds;
}

function extractBuildName(name) {
  return name.replace(
    "https://csci-526-spring-2025.github.io/csci-526-final-project-notimetodebug/",
    ""
  );
}
