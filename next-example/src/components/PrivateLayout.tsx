import { PrivateHeader } from "./PrivateHeader";
import PrivateSideNav from "./PrivateSideNav";

interface Props {
  children: React.ReactNode;
}

export default function PrivateLayout({ children }: Props) {
  return (
    <div className="flex w-full h-full">
      <PrivateSideNav />
      <div className="flex-1 min-h-screen bg-slate-100 text-slate-900 dark:bg-slate-900 dark:text-white">
        <PrivateHeader />
        <main className="px-4 py-2 bg-slate-100 text-slate-900 dark:bg-slate-900 dark:text-white sm:px-6 md:py-4 md:px-8">
          {children}
        </main>
      </div>
    </div>
  );
}
