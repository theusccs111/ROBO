export function getBracoDireitoCotovelo(): { label: string; value: number }[] {
  return [
    { label: 'Em Repouso', value: 0 },
    { label: 'Levemente Contraído', value: 1 },
    { label: 'Contraído', value: 2 },
    { label: 'Fortemente Contraído', value: 3 }
  ]
}

export function getBracoDireitoPulso(): { label: string; value: number }[] {
  return [
    { label: 'Rotação para -90º', value: 0 },
    { label: 'Rotação para -45', value: 1 },
    { label: 'Em Repouso', value: 2 },
    { label: 'Rotação para 45º', value: 3 },
    { label: 'Rotação para 90º', value: 4 },
    { label: 'Rotação para 135º', value: 5 },
    { label: 'Rotação para 180º', value: 6 }
  ]
}

export function getBracoEsquerdoCotovelo(): { label: string; value: number }[] {
  return [
    { label: 'Em Repouso', value: 0 },
    { label: 'Levemente Contraído', value: 1 },
    { label: 'Contraído', value: 2 },
    { label: 'Fortemente Contraído', value: 3 }
  ]
}

export function getBracoEsquerdoPulso(): { label: string; value: number }[] {
  return [
    { label: 'Rotação para -90º', value: 0 },
    { label: 'Rotação para -45', value: 1 },
    { label: 'Em Repouso', value: 2 },
    { label: 'Rotação para 45º', value: 3 },
    { label: 'Rotação para 90º', value: 4 },
    { label: 'Rotação para 135º', value: 5 },
    { label: 'Rotação para 180º', value: 6 }
  ]
}

export function getCabecaInclinacao(): { label: string; value: number }[] {
  return [
    { label: 'Para cima', value: 0 },
    { label: 'Em Repouso', value: 1 },
    { label: 'Para Baixo', value: 2 }
  ]
}

export function getCabecaRotacao(): { label: string; value: number }[] {
  return [
    { label: 'Rotação para -90º', value: 0 },
    { label: 'Rotação para -45', value: 1 },
    { label: 'Em Repouso', value: 2 },
    { label: 'Rotação para 45º', value: 3 },
    { label: 'Rotação para 90º', value: 4 }
  ]
}



