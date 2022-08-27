import PrivateLayout from "@/components/PrivateLayout";
import { NewRecipeForm } from "@/domain/recipes";
import { useRouter } from "next/router";

export default function NewRecipe() {
  const router = useRouter();

  return (
    <PrivateLayout>
      <div className="space-y-6">
        <button
          className="px-3 py-2 border rounded-md border-slate-700 dark:border-white"
          onClick={() => router.back()}
        >
          Back
        </button>
        <div className="">
          <h1 className="max-w-4xl text-2xl font-medium tracking-tight font-display text-slate-900 dark:text-gray-50 sm:text-4xl">
            Add a Recipe
          </h1>
          <div className="py-6">
            <NewRecipeForm />
          </div>
        </div>
      </div>
    </PrivateLayout>
  );
}
