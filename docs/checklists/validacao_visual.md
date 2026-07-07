# Checklist de Validacao Visual no Godot

Use este checklist no fim de cada sprint.

Politica completa: `docs/10_politica_validacao_visual.md`.

Godot local confirmado:

```text
C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64.exe
```

## Antes de abrir

- [ ] `dotnet restore TerraSangrada.csproj` executado quando necessario.
- [ ] `dotnet build TerraSangrada.csproj --no-restore` sem erros.
- [ ] Cena alvo identificada.
- [ ] Checklist da sprint aberto.

## No Godot

- [ ] Projeto abre sem erro critico.
- [ ] Cena principal roda.
- [ ] Nenhum script essencial aparece quebrado.
- [ ] Camera enquadra a acao.
- [ ] Player aparece.
- [ ] Inimigos aparecem, se a sprint exigir.
- [ ] Fundo nao atrapalha a leitura.
- [ ] Texto e UI nao sobrepoem elementos importantes, se houver UI.

## Gameplay

- [ ] Controles respondem.
- [ ] Colisao basica funciona.
- [ ] Ataques conectam quando esperado.
- [ ] Feedback visual de dano aparece.
- [ ] Nao ha travamento obvio.

## Fechamento

- [ ] Resultado registrado no documento da sprint.
- [ ] `docs/08_estado_atual.md` atualizado.
- [ ] Commit local criado com a sprint validada.
- [ ] Push enviado para o GitHub.
- [ ] Proximo passo definido somente apos o push.
