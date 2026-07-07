# Sprint 19 - Arsenal, Arquearia e Bleed

## Objetivo

Introduzir o loop de **flechas limitadas**, **sangramento** e base para **artefatos** e **upgrade de tacape** na Fase 1 — sem virar shooter.

Plano completo: `docs/13_sistema_de_arsenal_e_arquearia.md`.

---

## Escopo desta sprint (MVP jogavel)

### Aljava e flechas

- [ ] Cap de **5 flechas**; HUD mostra contagem.
- [ ] Pickup `ArrowPickup` no chao (+1 / feixe +3).
- [ ] `R` segurar = mira; soltar = disparo na direcao do `facing` + ajuste Y leve.
- [ ] Projétil 2.5D com alcance limitado (~220px).
- [ ] Custo: **8 stamina** por disparo; recovery 0.28s.

### Sangramento

- [ ] Componente `BleedStatus` em `EnemyBase`.
- [ ] Flecha aplica **Sangramento I** (0.5 DPS, 4s).
- [ ] Visual: mancha + wound existente escalada.
- [ ] Decal de sangue no chao ao tick (reusar sistema de decals).

### Tacape (fundacao)

- [ ] `WeaponProfile` com tier 0 (atual) e estrutura para tier 1+.
- [ ] HUD: label `Tacape Ritual` (placeholder upgrade).
- [ ] Sem mudanca de dano nesta sprint — so arquitetura.

### Artefato (1 tipo)

- [ ] `ArtifactPickup` — **Faca de Ferro Roubada** (3 usos).
- [ ] Tecla `U` troca modo artefato; proximo `J` = corte rapido com Sangramento II.
- [ ] Quebra apos 3 usos; feedback sonoro + particula.

### Direcao de fase

- [ ] `PhaseDirector` / Aldeia: 1 spawn de flechas + 1 artefato opcional no 2o encontro.

---

## Fora desta sprint

- Hemorragia II, tipos de flecha avancados.
- Tacape tier 1 com dano real (Sprint 20).
- Gancho, espinhos, clava quebrada.
- Upgrade permanente via memoria.

---

## Controles novos

| Tecla | Acao |
|-------|------|
| `R` | Mira e dispara flecha |
| `U` | Equipa/descarta artefato ativo |

---

## Classes / arquivos previstos

```
src/Prototype/
  QuiverInventory.cs
  ArrowProjectile.cs
  ArrowPickup.cs
  ArtifactPickup.cs
  ArtifactDefinition.cs
  BleedStatus.cs
  WeaponProfile.cs
  RangedCombat.cs        (ou extensao PlayerController)
  CombatHud.cs           (+ aljava, artefato, tacape)
```

---

## Balance inicial

| Parametro | Valor |
|-----------|-------|
| Aljava max | 5 |
| Dano flecha | 1 |
| Stamina/disparo | 8 |
| Bleed I DPS | 0.5 / 4s |
| Faca usos | 3 |
| Faca dano | 2 (combo finisher scale) |

---

## Testes de validacao

- [ ] Coletar flechas aumenta HUD; cap respeitado.
- [ ] Disparo sem flecha = feedback vermelho (stamina flash reuse).
- [ ] Inimigo com bleed perde vida ao longo do tempo e mancha o chao.
- [ ] Faca quebra no 3o hit e some do HUD.
- [ ] Tacape combos `J`/`K`/`E` inalterados sem artefato equipado.
- [ ] Build C# sem erros; validacao visual Godot.

---

## Dependencias

- Sprint 17 (movimento + facing) validada.
- Sprint 18 (pulo + combos) recomendada antes — mas 19 pode comecar em paralelo se 18 atrasar.

---

## Proxima sprint (20)

- Tacape Tier 1 (memoria ou pickup narrativo).
- Segundo artefato (Gancho ou Clava 1 uso).
- Hemorragia I em pesado `K` com tacape tier 2.

---

## Registro de validacao visual

Status atual: nao iniciada.
