# Sprint 23 - Golpes de Corrida (Final Fight)

## Objetivo

Golpes exclusivos enquanto o personagem **corre** (`A`/`D` 2x), no estilo **Final Fight** / **Cadillac Dinosaurs** — leitura clara de “estou em movimento ofensivo”.

## Contexto

Hoje existe `StartRunAttack()` apenas no `J` (golpe de entrada generico). Falta diferenciacao forte, animacao propria e variante pesada no `K`.

## Escopo proposto

### Corrida + `J` — golpe de entrada

- [ ] Animacao/sprite dedicada (soco chutado ou cotovelada enquanto desliza).
- [ ] Mantem momentum parcial na direcao da corrida.
- [ ] Knockback alto, dano leve+, recovery curto.
- [ ] Nao consome combo chain normal.

### Corrida + `K` — golpe pesado de corrida

- [ ] Variante pesada (joelhada, shoulder charge ou chute corrido).
- [ ] Custo de stamina extra (~6).
- [ ] Para a corrida ao conectar; recovery medio.
- [ ] Pode aplicar stagger em brutos.

### Game feel

- [ ] SFX distintos (whoosh + impacto).
- [ ] Hitstop levemente maior que golpe parado.
- [ ] Cancel: corrida → golpe sem voltar a idle.

### HUD / UX

- [ ] Flash sutil no sprite ao correr (opcional).
- [ ] Documentar no `08_estado_atual.md`.

## Referencia de design

| Jogo | Referencia |
|------|------------|
| Final Fight | Dash punch / running attack |
| Cadillac Dinosaurs | Golpes mantendo deslocamento |

## Fora desta sprint

- Combos encadeando apos golpe de corrida.
- Tacape tier afetando animacao.

## Registro de validacao visual

Status atual: nao iniciada.
