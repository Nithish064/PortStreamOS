import React from 'react';

const dummyManifests = [
  { id: "1", documentNumber: "MNF-2026-001", vesselName: "Ever Given", submissionDate: "2026-07-21T08:00:00", score: "Low", audit: false },
  { id: "2", documentNumber: "MNF-2026-002", vesselName: "Maersk Mc-Kinney", submissionDate: "2026-07-21T09:15:00", score: "Medium", audit: false },
  { id: "3", documentNumber: "MNF-2026-003", vesselName: "Seawise Giant", submissionDate: "2026-07-21T10:30:00", score: "Critical", audit: true }
];

const dummyRules = [
  { id: "1", name: "High Risk Origin", target: "OriginCountry", op: "Equals", val: "CountryX", risk: "High", active: true },
  { id: "2", name: "Heavy Cargo Audit", target: "WeightKg", op: "GreaterThan", val: "50000", risk: "Medium", active: true }
];

export default function CustomsDashboard() {
  return (
    <div className="flex flex-col lg:flex-row gap-8 animate-in fade-in duration-500">
      
      {/* MANIFEST DESK */}
      <section className="flex-1 glass-panel p-6">
        <div className="flex justify-between items-center mb-6">
          <h2 className="text-2xl font-semibold text-white">Live Manifest Risk Assessment</h2>
        </div>
        
        <div className="overflow-x-auto">
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="border-b border-slate-700 text-slate-400 text-sm">
                <th className="pb-3 font-medium">Manifest #</th>
                <th className="pb-3 font-medium">Vessel</th>
                <th className="pb-3 font-medium">Risk Score</th>
                <th className="pb-3 font-medium">Action</th>
              </tr>
            </thead>
            <tbody>
              {dummyManifests.map((m) => (
                <tr key={m.id} className="border-b border-slate-800 hover:bg-slate-800/50 transition-colors">
                  <td className="py-4 font-medium text-sky-400">{m.documentNumber}</td>
                  <td className="py-4 text-slate-300">{m.vesselName}</td>
                  <td className="py-4">
                    <span className={`px-2 py-1 rounded text-xs font-medium border ${
                      m.score === 'Critical' ? 'bg-red-500/20 text-red-400 border-red-500/30' :
                      m.score === 'Medium' ? 'bg-warning/20 text-warning border-warning/30' :
                      'bg-success/20 text-success border-success/30'
                    }`}>
                      {m.score}
                    </span>
                  </td>
                  <td className="py-4">
                    {m.audit ? (
                      <button className="bg-red-500 hover:bg-red-600 text-white text-xs px-3 py-1.5 rounded transition-colors shadow-lg shadow-red-500/20">
                        Initiate Audit
                      </button>
                    ) : (
                      <span className="text-slate-500 text-xs">Cleared</span>
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </section>

      {/* RULE BUILDER SIDEBAR */}
      <section className="lg:w-1/3 flex flex-col gap-6">
        
        <div className="glass-panel p-6">
          <h3 className="text-xl font-semibold text-white mb-4 border-b border-slate-700 pb-2">Compliance Rule Builder</h3>
          <form className="flex flex-col gap-4 text-sm">
            <div>
              <label className="block text-slate-400 mb-1">Rule Name</label>
              <input type="text" className="w-full bg-slate-800/50 border border-slate-700 rounded p-2 text-white outline-none focus:border-sky-500 transition-colors" placeholder="e.g., Toxic Cargo Check" />
            </div>
            <div className="grid grid-cols-2 gap-3">
              <div>
                <label className="block text-slate-400 mb-1">Target Field</label>
                <select className="w-full bg-slate-800/50 border border-slate-700 rounded p-2 text-white outline-none focus:border-sky-500">
                  <option>OriginCountry</option>
                  <option>WeightKg</option>
                  <option>HSCode</option>
                </select>
              </div>
              <div>
                <label className="block text-slate-400 mb-1">Operator</label>
                <select className="w-full bg-slate-800/50 border border-slate-700 rounded p-2 text-white outline-none focus:border-sky-500">
                  <option>Equals</option>
                  <option>Contains</option>
                  <option>GreaterThan</option>
                </select>
              </div>
            </div>
            <div>
              <label className="block text-slate-400 mb-1">Value</label>
              <input type="text" className="w-full bg-slate-800/50 border border-slate-700 rounded p-2 text-white outline-none focus:border-sky-500 transition-colors" placeholder="Value to match" />
            </div>
            <button type="button" className="w-full bg-sky-500 hover:bg-sky-400 text-white font-medium py-2 rounded transition-colors mt-2">
              + Add Rule
            </button>
          </form>
        </div>

        <div className="glass-panel p-6">
          <h3 className="text-sm font-medium text-slate-400 mb-4 uppercase tracking-wider">Active Rules</h3>
          <div className="flex flex-col gap-3">
            {dummyRules.map(r => (
              <div key={r.id} className="bg-slate-800/80 border border-slate-700 p-3 rounded flex justify-between items-center">
                <div>
                  <p className="text-sm font-medium text-slate-200">{r.name}</p>
                  <p className="text-xs text-slate-500 mt-0.5">{r.target} {r.op} {r.val}</p>
                </div>
                <span className={`w-2 h-2 rounded-full ${r.active ? 'bg-success' : 'bg-slate-500'}`}></span>
              </div>
            ))}
          </div>
        </div>

      </section>
    </div>
  );
}
