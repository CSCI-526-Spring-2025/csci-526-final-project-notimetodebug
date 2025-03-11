import { fetchData } from "./fetchData.js";

let data;
let selectedSessionURL = undefined;
let damageStats;
let damageChart = undefined;

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
  console.log(damageStats);
  drawDamageStatsChart(damageStats);
}

function drawDamageStatsChart(data) {
  const ctx = document.getElementById("damage-chart-display");

  damageChart?.destroy();

  let labels = [
    ...new Set(
      Object.keys(data)
        .map((k) => Object.keys(data[k]))
        .flat()
    ),
  ];

  let enemyDamagePerLevel = Object.keys(data)
    .filter((damageType) => damageType.startsWith("GroundEnemy"))
    .reduce((p, c, i, a) => {
      Object.keys(data[c]).forEach((level) => {
        if (p[level]) {
          p[level] += data[c][level];
        } else {
          p[level] = data[c][level];
        }
      });

      return p;
    }, {});

  Object.keys(data)
    .filter((damageType) => damageType.startsWith("GroundEnemy"))
    .forEach((key) => delete data[key]);

  data["GroundedEnemy:Bullets"] = enemyDamagePerLevel;

  let chartData = {
    labels: labels,
    datasets: Object.keys(data).map((key, i, a) => {
      return {
        label: key,
        data: labels.map((label) => (data[key] ? data[key][label] ?? 0 : 0)),
      };
    }),
  };

  damageChart = new Chart(ctx, {
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
      if (!p[c.eventData]) p[c.eventData] = {};
      if (!p[c.eventData][c.levelName]) p[c.eventData][c.levelName] = 0;

      p[c.eventData][c.levelName] += 1;

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
