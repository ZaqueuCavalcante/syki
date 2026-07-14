export interface InstitutionConfig {
  id: number
  noteLimit: number // nota mínima para aprovação na disciplina (de 0 a 10)
  frequencyLimit: number // frequência mínima para não reprovar por falta (de 0% a 100%)
}
