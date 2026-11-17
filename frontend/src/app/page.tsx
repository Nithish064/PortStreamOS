import { fetchVessels } from "@/services/api";

export const dynamic = 'force-dynamic';

export default async function Home() {
  const vessels = await fetchVessels();
  
  // Dummy data for presentation if backend isn't running
  const displayVessels = vessels.length > 0 ? vessels : [
    { id: "1", name: "Ever Given", imoNumber: "9811000", type: 0, status: 1, eta: "2026-07-21T10:00:00" },
    { id: "2", name: "Maersk Mc-Kinney", imoNumber: "9619907", type: 0, status: 0, eta: "2026-07-21T14:30:00" },
    { id: "3", name: "Seawise Giant", imoNumber: "7381154", type: 1, status: 2, eta: "2026-07-20T08:00:00" }
  ];

  return (
    <div className="flex flex-col gap-8 animate-in fade-in duration-500">
      
      <section className="glass-panel p-6">
        <div className="flex justify-between items-center mb-6">
          <h2 className="text-2xl font-semibold text-white">Active Channel Queue</h2>
          <span className="bg-sky-500/20 text-sky-400 py-1 px-3 rounded-full text-sm font-medium border border-sky-500/30">
            {displayVessels.length} Vessels Tracking
          </span>
        </div>
        
        <div className="overflow-x-auto">
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="border-b border-slate-700 text-slate-400 text-sm">
                <th className="pb-3 font-medium">Vessel Name</th>
                <th className="pb-3 font-medium">IMO</th>
                <th className="pb-3 font-medium">Status</th>
                <th className="pb-3 font-medium">ETA</th>
              </tr>
            </thead>
            <tbody>
              {displayVessels.map((v: any) => (
                <tr key={v.id} className="border-b border-slate-800 hover:bg-slate-800/50 transition-colors">
                  <td className="py-4 font-medium">{v.name}</td>
                  <td className="py-4 text-slate-400">{v.imoNumber}</td>
                  <td className="py-4">
                    <span className={`px-2 py-1 rounded text-xs font-medium ${
                      v.status === 1 ? 'bg-warning/20 text-warning border border-warning/30' :
                      v.status === 2 ? 'bg-success/20 text-success border border-success/30' :
                      'bg-sky-500/20 text-sky-400 border border-sky-500/30'
                    }`}>
                      {v.status === 1 ? 'In Queue' : v.status === 2 ? 'Docked' : 'Approaching'}
                    </span>
                  </td>
                  <td className="py-4 text-slate-300">{new Date(v.eta).toLocaleString()}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </section>

      <section className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Kanban Board Placeholder for Berth Allocation */}
        {['Queue', 'Berth A1', 'Berth A2'].map((col, idx) => (
          <div key={idx} className="glass-panel p-4 kanban-column">
            <h3 className="font-semibold text-lg mb-4 text-slate-300 border-b border-slate-700 pb-2">{col}</h3>
            {idx === 0 && (
              <div className="bg-slate-800/80 p-4 rounded-lg mb-3 border border-slate-700 kanban-card">
                <h4 className="font-medium text-sky-400">Ever Given</h4>
                <p className="text-xs text-slate-400 mt-1">Cargo | ETA: 10:00 AM</p>
              </div>
            )}
            {idx === 1 && (
              <div className="bg-slate-800/80 p-4 rounded-lg mb-3 border border-slate-700 kanban-card border-l-4 border-l-success">
                <h4 className="font-medium text-success">Seawise Giant</h4>
                <p className="text-xs text-slate-400 mt-1">Tanker | Docked</p>
              </div>
            )}
          </div>
        ))}
      </section>
      
    </div>
  );
}
