import React from 'react';

const dummyEmissions = [
  { id: "1", location: "North Gate", aqi: 45, co2: 380, noise: 70, status: "Good" },
  { id: "2", location: "Berth 1", aqi: 110, co2: 520, noise: 88, status: "Warning" }, // Non-compliant
  { id: "3", location: "Yard Block C", aqi: 65, co2: 410, noise: 75, status: "Good" }
];

const dummyShifts = [
  { id: "1", name: "John Doe", role: "Crane Operator", hours: 4.5, safetyChecked: true, alert: false },
  { id: "2", name: "Jane Smith", role: "Stevedore", hours: 12.5, safetyChecked: true, alert: true }, // Overtime!
  { id: "3", name: "Mike Johnson", role: "Gantry Tech", hours: 8.0, safetyChecked: false, alert: true } // Gear not checked!
];

export default function OperationsDashboard() {
  return (
    <div className="flex flex-col gap-8 animate-in fade-in duration-500">
      
      {/* ENVIRONMENTAL COMPLIANCE */}
      <section className="glass-panel p-6">
        <div className="flex justify-between items-center mb-6">
          <h2 className="text-2xl font-semibold text-white flex items-center gap-2">
            🌿 Green Port Compliance
          </h2>
          <span className="bg-success/20 text-success py-1 px-3 rounded-full text-sm font-medium border border-success/30">
            Live Sensors
          </span>
        </div>
        
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
          {dummyEmissions.map((sensor) => (
            <div key={sensor.id} className="bg-slate-800/80 p-5 rounded-xl border border-slate-700 hover:border-sky-500/50 transition-colors">
              <h3 className="text-lg font-bold text-white mb-4">{sensor.location}</h3>
              
              <div className="flex flex-col gap-3">
                <div className="flex justify-between items-center">
                  <span className="text-sm text-slate-400">AQI</span>
                  <span className={`font-mono font-medium ${sensor.aqi > 100 ? 'text-warning' : 'text-success'}`}>{sensor.aqi}</span>
                </div>
                
                <div className="flex justify-between items-center">
                  <span className="text-sm text-slate-400">CO2 Emissions</span>
                  <span className={`font-mono font-medium ${sensor.co2 > 500 ? 'text-warning' : 'text-success'}`}>{sensor.co2} ppm</span>
                </div>

                <div className="flex justify-between items-center">
                  <span className="text-sm text-slate-400">Noise Level</span>
                  <span className={`font-mono font-medium ${sensor.noise > 85 ? 'text-warning' : 'text-success'}`}>{sensor.noise} dB</span>
                </div>
              </div>
              
              <div className={`mt-4 pt-3 border-t border-slate-700 text-sm font-bold text-center rounded-b-lg ${sensor.status === 'Good' ? 'text-success bg-success/5' : 'text-warning bg-warning/10 animate-pulse'}`}>
                {sensor.status === 'Good' ? 'All Systems Nominal' : 'Compliance Warning'}
              </div>
            </div>
          ))}
        </div>
      </section>

      {/* WORKFORCE SCHEDULING */}
      <section className="glass-panel p-6">
        <div className="flex justify-between items-center mb-6">
          <h2 className="text-2xl font-semibold text-white flex items-center gap-2">
            👷 Workforce Safety & Scheduling
          </h2>
        </div>
        
        <div className="overflow-x-auto">
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="border-b border-slate-700 text-slate-400 text-sm">
                <th className="pb-3 font-medium">Worker Name</th>
                <th className="pb-3 font-medium">Role</th>
                <th className="pb-3 font-medium">Shift Duration</th>
                <th className="pb-3 font-medium">Safety Gear</th>
                <th className="pb-3 font-medium">Status</th>
              </tr>
            </thead>
            <tbody>
              {dummyShifts.map((s) => (
                <tr key={s.id} className="border-b border-slate-800 hover:bg-slate-800/50 transition-colors">
                  <td className="py-4 font-medium text-white">{s.name}</td>
                  <td className="py-4 text-slate-300">{s.role}</td>
                  <td className="py-4">
                    <span className={`font-mono ${s.hours > 12 ? 'text-red-400 font-bold' : 'text-slate-300'}`}>
                      {s.hours} Hrs
                    </span>
                  </td>
                  <td className="py-4">
                    {s.safetyChecked ? (
                      <span className="text-success">✔ Verified</span>
                    ) : (
                      <span className="text-red-400 font-bold">✖ Missing</span>
                    )}
                  </td>
                  <td className="py-4">
                    {s.alert ? (
                      <span className="px-2 py-1 rounded text-xs font-bold bg-red-500/20 text-red-400 border border-red-500/30 animate-pulse">
                        ACTION REQUIRED
                      </span>
                    ) : (
                      <span className="px-2 py-1 rounded text-xs font-medium bg-success/20 text-success border border-success/30">
                        Compliant
                      </span>
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
