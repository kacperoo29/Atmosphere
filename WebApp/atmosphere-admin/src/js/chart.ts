import * as chartjs from "chart.js/auto";
import "chartjs-adapter-moment";

export function draw_chart(element_id: string, data: any, type: string) {
  let ctx = document.getElementById(element_id) as HTMLCanvasElement;
  console.log(ctx);
  console.log(data);
  if (ctx === null) {
    console.error("Could not find element with id " + element_id);
    return;
  }

  let dataMap = data as Map<string, number>;
  // map keys to date
  let dates = Array.from(dataMap.keys());
  let values = Array.from(dataMap.values());

  // sort by dates
  let sorted = dates
    .map((date, i) => [date, values[i]])
    .sort((a, b) => new Date(a[0]).getTime() - new Date(b[0]).getTime());

  let chartData = {
    labels: sorted.map((a) => a[0]) as string[],
    datasets: [
      {
        label: type,
        data: sorted.map((a) => a[1]) as number[],
        fill: false,
        borderColor: "rgb(75, 192, 192)",
        tension: 0.1,
      },
    ],
  };

  let chartOpts: chartjs.ChartConfiguration = {
    type: "line",
    data: chartData,
    options: {
      scales: {
        x: {
          type: "time",
        },
      },
    },
  };

  var myChart = new chartjs.Chart(ctx, chartOpts);
  myChart.update("show");
}
