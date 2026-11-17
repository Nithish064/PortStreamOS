import React from 'react';

// This would normally fetch from our Ocelot gateway (/api/yard/heatmap)
// We use dummy data to present the visual heatmap component 
const dummyYardData = [
  { id: "1", blockName: "Block A", maxCapacity: 500, currentCount: 450, utilizationPercentage: 90 },
  { id: "2", blockName: "Block B", maxCapacity: 500, currentCount: 200, utilizationPercentage: 40 },
  { id: "3", blockName: "Block C", maxCapacity: 500, currentCount: 490, utilizationPercentage: 98 },
  { id: "4", blockName: "Block D", maxCapacity: 300, currentCount: 50, utilizationPercentage: 16 },
  { id: "5", blockName: "Block E", maxCapacity: 400, currentCount: 300, utilizationPercentage: 75 },
  { id: "6", blockName: "Block F", maxCapacity: 400, currentCount: 395, utilizationPercentage: 98.75 }
];

const dummyReeferData = [
  { id: "1", containerNumber: "MSKU1234567", temp: -18.5, target: -18.0, power: true },
  { id: "2", containerNumber: "CMAU7654321", temp: -2.0, target: -18.0, power: false }, // Alert!
  { id: "3", containerNumber: "HLCU0987654", temp: 4.2, target: 4.0, power: true }
];

export default function YardDashboard() {
  return (
    <div className="flex flex-col gap-8 animate-in fade-in duration-500">
      
      {/* HEATMAP SECTION */}
      <section className="glass-panel p-6">
        <div className="flex justify-between items-center mb-6">
          <h2 className="text-2xl font-semibold text-white">Yard Capacity Heat-Matrix</h2>
          <span className="bg-sky-500/20 text-sky-400 py-1 px-3 rounded-full text-sm font-medium border border-sky-500/30">
            Powered by Redis
          </span>
        </div>
        
        <div className="grid grid-cols-2 md:grid-cols-3 gap-4">
          {dummyYardData.map((bin) => {
            // Determine heat color based on utilization
            let heatClass = "bg-slate-800/80 border-slate-700";
            if (bin.utilizationPercentage > 85) heatClass = "bg-red-500/20 border-red-500/50";
            else if (bin.utilizationPercentage > 60) heatClass = "bg-warning/20 border-warning/50";
            else heatClass = "bg-success/20 border-success/50";

            return (
              <div key={bin.id} className={`p-4 rounded-xl border ${heatClass} transition-all hover:scale-[1.02]`}>
                <h3 className="font-bold text-lg mb-2">{bin.blockName}</h3>
                <div className="flex justify-between text-sm text-slate-300 mb-2">
                  <span>{bin.currentCount} / {bin.maxCapacity} TEU</span>
                  <span className="font-bold text-white">{bin.utilizationPercentage.toFixed(1)}%</span>
                </div>
                {/* Progress bar */}
                <div className="w-full bg-slate-900 rounded-full h-2">
                  <div 
                    className={`h-2 rounded-full ${bin.utilizationPercentage > 85 ? 'bg-red-500' : bin.utilizationPercentage > 60 ? 'bg-warning' : 'bg-success'}`} 
                    style={{ width: `${bin.utilizationPercentage}%` }}
                  ></div>
                </div>
              </div>
            );
          })}
        </div>
      </section>

      {/* REEFER MONITORING SECTION */}
      <section className="glass-panel p-6">
        <h2 className="text-2xl font-semibold text-white mb-6">Reefer Temperature Grid</h2>
        <div className="overflow-x-auto">
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="border-b border-slate-700 text-slate-400 text-sm">
                <th className="pb-3 font-medium">Container ID</th>
                <th className="pb-3 font-medium">Current Temp</th>
                <th className="pb-3 font-medium">Target Temp</th>
                <th className="pb-3 font-medium">Power Status</th>
              </tr>
            </thead>
            <tbody>
              {dummyReeferData.map((r) => (
                <tr key={r.id} className="border-b border-slate-800 hover:bg-slate-800/50 transition-colors">
                  <td className="py-4 font-medium">{r.containerNumber}</td>
                  <td className="py-4">
                    {/* Visual alert if temp deviates too much */}
                    <span className={`font-mono text-lg ${Math.abs(r.temp - r.target) > 3 ? 'text-red-400 font-bold animate-pulse' : 'text-slate-200'}`}>
                      {r.temp.toFixed(1)}°C
                    </span>
                  </td>
                  <td className="py-4 text-slate-400 font-mono">{r.target.toFixed(1)}°C</td>
                  <td className="py-4">
                    {r.power ? (
                      <span className="px-2 py-1 rounded text-xs font-medium bg-success/20 text-success border border-success/30">Connected</span>
                    ) : (
                      <span className="px-2 py-1 rounded text-xs font-medium bg-red-500/20 text-red-400 border border-red-500/30 animate-pulse">Disconnected</span>
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </section>
      
    </div>
  );
}
