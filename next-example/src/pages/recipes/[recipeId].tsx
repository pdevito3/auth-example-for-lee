import PrivateLayout from "@/components/PrivateLayout";
import { useRouter } from "next/router";

export default function RecipeForm() {
  const router = useRouter();
  const { recipeId } = router.query;

  return (
    <PrivateLayout>
      <div className="space-y-6">
        <button
          className="px-3 py-2 border border-white rounded-md"
          onClick={() => router.back()}
        >
          Back
        </button>
        <div className="">
          <div className="flex items-center justify-between">
            <h1 className="max-w-4xl text-2xl font-medium tracking-tight font-display text-slate-900 dark:text-gray-50 sm:text-4xl">
              Recipes Edit Form
            </h1>
          </div>
          <div className="">TBD Form for {recipeId}</div>
        </div>
      </div>
    </PrivateLayout>
  );
}
